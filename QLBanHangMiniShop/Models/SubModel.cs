using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace QLBanHangMiniShop.Models
{
    [MetadataType(typeof(ChungLoai.ChungLoaiMetadata))]
    public partial class ChungLoai
    {
        class ChungLoaiMetadata
        {
            [Display(Name ="Tên nhóm sản phẩm")]
            [MaxLength(50,ErrorMessage ="{0} không được quá {1}")]
            [Required(ErrorMessage ="{0} không được để trống")]
            public string Ten;
        }
    }
}