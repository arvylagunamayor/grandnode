using Grand.Api.Commands.Models.Mobile.Catalog;
using Grand.Core.Caching;
using Grand.Domain.Catalog;
using Grand.Domain.Media;
using Grand.Services.Catalog;
using Grand.Services.Localization;
using Grand.Services.Media;
using Grand.Services.Seo;
using MediatR;
using RobustErrorHandler.Core;
using RobustErrorHandler.Core.Errors;
using RobustErrorHandler.Core.SuccessCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grand.Api.Commands.Handlers.Mobile.Catalog
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryCommand, Either<Error, Success<IList<CategoryModel>>>>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICategoryService _categoryService;
        private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IMediator _mediator;
        private readonly MediaSettings _mediaSettings;
        private readonly CatalogSettings _catalogSettings;

        public GetCategoryHandler(
            ICacheManager cacheManager,
            ICategoryService categoryService,
            IPictureService pictureService,
            IProductService productService,
            IMediator mediator,
            MediaSettings mediaSettings,
            CatalogSettings catalogSettings)
        {
            _cacheManager = cacheManager;
            _categoryService = categoryService;
            _pictureService = pictureService;
            _productService = productService;
            _mediator = mediator;
            _mediaSettings = mediaSettings;
            _catalogSettings = catalogSettings;
        }

        public async Task<Either<Error, Success<IList<CategoryModel>>>> Handle(GetCategoryCommand request, CancellationToken cancellationToken)
        {
            IList<CategoryModel> result = new List<CategoryModel>();

            // get all categories
            var allCategories = await _categoryService.GetAllCategories();

            //filter and sort category by display order
            var categories = allCategories.OrderBy(c => c.DisplayOrder).ToList();


            foreach(var category in categories)
            {
                var picture = await _pictureService.GetPictureById(category.PictureId);

                var categoryModel = new CategoryModel {
                    Id = category.Id,
                    Name = category.Name,
                    IncludeInTopMenu = category.IncludeInTopMenu,
                    Flag = category.Flag,
                    FlagStyle = category.FlagStyle,
                    Icon = category.Icon,
                    ImageUrl = await _pictureService.GetPictureUrl(picture, _mediaSettings.CategoryThumbPictureSize),
                    GenericAttributes = category.GenericAttributes
                };

                result.Add(categoryModel);
            }

            if (result.Count > 0)
                return Result.Success(result);
            else
                return Result.NoContent<IList<CategoryModel>>(result);
        }

    }
}
