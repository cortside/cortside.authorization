#pragma warning disable CS1591 // Missing XML comments

using Cortside.Authorization.Dto;
using Cortside.Authorization.WebApi.Models;

namespace Cortside.Authorization.WebApi.Mappers {
    public class AddressModelMapper {
        public AddressModel Map(AddressDto dto) {
            if (dto == null) {
                return null;
            }

            return new AddressModel() {
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                ZipCode = dto.ZipCode
            };
        }
    }
}
