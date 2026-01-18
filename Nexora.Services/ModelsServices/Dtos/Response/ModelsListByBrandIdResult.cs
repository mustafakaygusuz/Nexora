using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.Services.ModelsServices.Dtos.Response
{
    public class ModelsListByBrandIdResult
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
