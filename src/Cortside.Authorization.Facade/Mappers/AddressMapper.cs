using Cortside.Authorization.Domain.Entities;
using Cortside.Authorization.Dto;

namespace Cortside.Authorization.Facade.Mappers {
    public class AddressMapper {
        public AddressDto MapToDto(Address entity) {
            if (entity == null) {
                return null;
            }

            return new AddressDto() {
                Street = entity.Street,
                City = entity.City,
                State = entity.State,
                Country = entity.Country,
                ZipCode = entity.ZipCode
            };
        }
    }
}
