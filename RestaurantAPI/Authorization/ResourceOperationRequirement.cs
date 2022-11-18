using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation ResouroceOperation { get; }

        public ResourceOperationRequirement(ResourceOperation resouroceOperation)
        {
            ResouroceOperation = resouroceOperation;
        }
    }
}
