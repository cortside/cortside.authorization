using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Cortside.Authorization.Facade;
using Cortside.Authorization.WebApi.Controllers;
using Cortside.Health.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cortside.Authorization.WebApi.IntegrationTests.Tests {
    public class DependencyInjectionTest : IClassFixture<IntegrationFixture> {
        private readonly IntegrationFixture webApi;
        private readonly ITestOutputHelper testOutputHelper;

        public DependencyInjectionTest(IntegrationFixture webApi, ITestOutputHelper testOutputHelper) {
            this.webApi = webApi;
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void VerifyControllerResolution() {
            var controllersAssembly = typeof(HealthController).Assembly;
            var controllers = controllersAssembly.ExportedTypes.Where(x => typeof(ControllerBase).IsAssignableFrom(x) && !x.IsAbstract).ToList();

            controllersAssembly = typeof(SettingsController).Assembly;
            controllers.AddRange(controllersAssembly.ExportedTypes.Where(x => typeof(ControllerBase).IsAssignableFrom(x) && !x.IsAbstract));

            var activator = webApi.Services.GetService<IControllerActivator>();
            var sp = webApi.Services.GetService<IServiceProvider>();
            var serviceProvider = sp.CreateScope().ServiceProvider;
            var errors = new Dictionary<Type, Exception>();

            var count = 0;
            var min = long.MaxValue;
            var max = long.MinValue;
            long total = 0;
            var slowest = string.Empty;

            foreach (var controllerType in controllers) {
                try {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var actionContext = new ActionContext(
                        new DefaultHttpContext {
                            RequestServices = serviceProvider
                        },
                        new RouteData(),
                        new ControllerActionDescriptor {
                            ControllerTypeInfo = controllerType.GetTypeInfo()
                        });
                    var controller = activator.Create(new ControllerContext(actionContext));
                    stopwatch.Stop();

                    if (stopwatch.ElapsedMilliseconds > max) {
                        max = stopwatch.ElapsedMilliseconds;
                        slowest = controller.GetType().ToString();
                    }
                    if (stopwatch.ElapsedMilliseconds < min) {
                        min = stopwatch.ElapsedMilliseconds;
                    }
                    count++;
                    total += stopwatch.ElapsedMilliseconds;

                    if (stopwatch.ElapsedMilliseconds > 100) {
                        testOutputHelper.WriteLine($"Resolved controller {controller.GetType()} in {stopwatch.ElapsedMilliseconds}ms");
                        testOutputHelper.WriteLine($"Slowest: {slowest}");
                    }
                } catch (Exception e) {
                    testOutputHelper.WriteLine($"Failed to resolve controller {controllerType} due to {e}");
                    errors.Add(controllerType, e);
                }
            }

            Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors.Select(x => $"Failed to resolve controller {x.Key.Name} due to {x.Value}")));
        }

        [Fact]
        public void VerifyFacadeResolution() {
            var sp = webApi.Services.GetService<IServiceProvider>();
            var serviceProvider = sp.CreateScope().ServiceProvider;

            int count = FindAndVerifyTypeResolution(typeof(PolicyFacade), serviceProvider);

            // assert that types were found and no exceptions happened before now
            count.Should().BePositive();
        }


        private int FindAndVerifyTypeResolution(Type t, IServiceProvider serviceProvider) {
            var types = new HashSet<Type>();
            AddTypes(types, t);

            var count = 0;
            var min = long.MaxValue;
            var max = long.MinValue;
            long total = 0;

            foreach (var type in types) {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var service = serviceProvider.GetRequiredService(type);
                stopwatch.Stop();

                if (stopwatch.ElapsedMilliseconds > max) {
                    max = stopwatch.ElapsedMilliseconds;
                }

                if (stopwatch.ElapsedMilliseconds < min) {
                    min = stopwatch.ElapsedMilliseconds;
                }

                count++;
                total += stopwatch.ElapsedMilliseconds;

                testOutputHelper.WriteLine($"Resolved type {service.GetType()} in {stopwatch.ElapsedMilliseconds}ms");
            }

            return count;
        }

        private void AddTypes(HashSet<Type> types, Type t) {
            if (types.Contains(t)) {
                return;
            }

            foreach (ConstructorInfo item in t.GetConstructors()) {
                if (!item.IsPublic || item.IsStatic) {
                    continue;
                }

                foreach (var p in item.GetParameters()) {
                    types.Add(p.ParameterType);
                    AddTypes(types, p.ParameterType);
                }
            }
        }
    }
}
