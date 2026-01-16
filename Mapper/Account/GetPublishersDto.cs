using News_App.Dto.Account;
using News_App.Mapper.newspages;
using News_App.Models;

namespace News_App.Mapper.Account
{
    public static class GetPublishersDto
    {
         public static  GetPublisherDto ToGetPublishersDto(this User user) { 
            var Publishers= new GetPublisherDto{
                Id=user.Id,
                Name=user.UserName,
                Email=user.Email,
                newspages=user.NewsPagesOfPublisher?.Select(n=>n.ToNewsPageDto2()).ToList(),
            };
            return Publishers;
            
        }
    }
}
