using Grand.Core.Models;

namespace Grand.Api.Commands.Models.Mobile.Catalog
{
    public partial class ProductSpecificationModel : BaseModel
    {
        public string SpecificationAttributeId { get; set; }

        public string SpecificationAttributeName { get; set; }

        //this value is already HTML encoded
        public string ValueRaw { get; set; }
        public string ColorSquaresRgb { get; set; }
    }
}