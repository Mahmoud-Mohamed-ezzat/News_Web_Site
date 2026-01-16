using News_App.Dto.newsPage;
using News_App.Models;

namespace News_App.Mapper.newspages
{
    public static class newspageMapper
    {
        public static Newspage TocreateNewspagewithadminIdDTO(this createNewspagewithadminIdDTO model,string Id)
        {
            return new Newspage
            {
                Name = model.Name,
                Iscreated = model.Iscreated,
                AdminId = Id,
            };
        }
        public static Newspage ToCreatenewspageforAdminOfPage(this createnewspageforAdminOfPage model)
        {
            return new Newspage
            {
                Name = model.Name,
                Iscreated = model.Iscreated,
                AdminId = model.AdminId,
            };
        }

        public static Newspage ToCreateNewsPages(this CreateNewspageDto model)
        {
            return new Newspage
            {
                Name = model.Name,
                Iscreated = model.Iscreated,
                AdminId = model.AdminId, // Uncommented this line
            };
        }
        public static UpdateNewsDto ToupdateNewsDto(this Newspage model)
        {
            return new UpdateNewsDto
            {
                Name = model.Name,
                Iscreated = model.Iscreated,
                AdminId = model.AdminId,
            };
        }
        public static newspageDtowithposts ToNewsPageDtowithposts(this Newspage model)
        {
            return new newspageDtowithposts
            {
                Id = model.Id,
                Name = model.Name,
                Iscreated = model.Iscreated,
                AdminId = model.AdminId,
                Posts = model.Posts,
            };
        }
        public static newspageDto2 ToNewsPageDto2(this Newspage model)
        {
            return new newspageDto2
            {
                Name = model.Name,
                Iscreated = model.Iscreated,
                AdminId = model?.AdminId,
                AdminName = model.Admin?.UserName,
            };
        }
        // Fixed: Renamed this method to avoid duplicate method names
        public static newspageDto ToNewsPageDto(this Newspage model)
        {
            return new newspageDto
            {
                Id = model.Id,
                Name = model.Name,
                Iscreated = model.Iscreated,
                AdminId = model?.AdminId,
            };
        }
        public static newspageDtowithposts ToNewsPageDtoWithPosts(this Newspage model)
        {
            return new newspageDtowithposts
            {
                Id = model.Id,
                Name = model.Name,
                Iscreated = model.Iscreated,
                AdminId = model.AdminId,
                Posts = model.Posts,
            };
        }
        public static newspageDtowithpublisher ToNewsPageDtoWithpublisher(this Newspage model)
        {
            return new newspageDtowithpublisher
            {
                Id = model.Id,
                Name = model.Name,
                Iscreated = model.Iscreated,
                AdminId = model.AdminId,
                Publishers = model.Publishers
            };
        }
        public static NewsPageDto3 TonewspageDto3(this Newspage model)
        {

            return new NewsPageDto3
            {
                Id = model.Id,
                Name= model.Name,
                AdminId= model.AdminId,
                AdminName=model.Admin.UserName,
            };
        }

    }
}