using GraduationProject_Core.Dtos.Sponsor;
using GraduationProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
    public interface ISponsorRepository
    {
       // Task<bool> AddSponsorToCompetitionAsync(AddSponsorToCompetitionDto dto);
     
        Task<List<Sponsorl>> GetSponsorsByCompetitionIdAsync(int competitionId);
        Task<List<SponsorDto>> GetSponsorsByNameAsync(string name);
        //Task<bool> RAddSponsorToCompetitionAsync(AddSponsorToCompetitionDtoe dto);
        Task<List<SponsorDto>> GetAllSponsorsAsync();
        Task<bool> UpdateSponsorAsync(int sponsorId, UpdateSponsorDto dto);
        //Task<bool> DeleteSponsorAsync(int sponsorId);
        Task<bool> RemoveSponsorFromCompetitionAsync(int sponsorId, int competitionId);
        Task<bool> RAddSponsorToCompetitionAsync(int competitionId, int sponsorId);

        Task<bool> AddSponsorToCompetitionAsync(int competitionId, AddSponsorToCompetitionDto dto);
       
        }
}
