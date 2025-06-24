using ECommercePlatform.Domain.Entities;
using ECommercePlatform.Logic.CommonService;
using ECommercePlatform.Shared.Constants;
using ECommercePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Odai.DataModel;
using Odai.Domain.Extension;
using Odai.Shared.Auth;

namespace ECommercePlatform.Logic.Services.Setting
{
    public class SettingService : BaseService<Settings>, ISettingService
    {
        public SettingService(OdaiDbContext context) : base(context)
        {
        }

        public async Task<Response<SettingsModel>> AddSetting(SettingsModel model)
        {
            var models = await Get(c => c.Id == model.Id).FirstOrDefaultAsync();
            await Delete(models);

            UpladFileModel filePath = new UpladFileModel();
            if (model.ImagePath != null)
            {

                filePath = await Extension.UploadFile(model.ImagePath);
            }


            var setting = new Settings
            {

                NameEn = model.NameEn,
                NameAr = model.NameAr,
                FilePath = filePath.FileName,
                ContentType = filePath.ContentType,
                CreatonDate = DateTime.Now,

            };
            await Insert(setting);
            await SaveChangesAsync();
            var resultModel = new SettingsModel
            {
                Id = setting.Id,
                NameEn = setting.NameEn,
                NameAr = setting.NameAr
            };

            return new Response<SettingsModel>(resultModel, HttpStatusCodes.Created, ResponseMessages.SavedSuccess);
        }

        public async Task<Response<SettingsModel>> GetSetting()
        {
            var setting = await GetAll().FirstOrDefaultAsync();
            if (setting != null)
            {

                var result = new SettingsModel
                {
                    Id = setting.Id,
                    NameEn = setting.NameEn,
                    NameAr = setting.NameAr,
                    ImageUrl = $"https://localhost:7036/Images/{Path.GetFileName(setting.FilePath)}"
                };
                return new Response<SettingsModel>(result, HttpStatusCodes.OK, ResponseMessages.OperationSucceeded);
            }
            return new Response<SettingsModel>("Setting not found", HttpStatusCodes.NotFound);
        }
    }
}
