﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.CompetitionImage
{
   public  class AddCompetitionImageDto
    {
      //  [Required]
        //public int CompetitionId { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
