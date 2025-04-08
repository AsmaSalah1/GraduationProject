using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.CompetitionImage
{
    public class GetImagesByCompetitionDto
    {
       
        public string ImageName { get; set; }
        public int CompetitionImagesId { get; set; }
    }
}
