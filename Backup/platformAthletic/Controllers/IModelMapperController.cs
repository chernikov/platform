using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Mappers;

namespace platformAthletic.Controllers
{
    public interface IModelMapperController
    {
        IMapper ModelMapper { get; }
    }
}
