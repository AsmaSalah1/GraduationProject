using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReTableImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_University_UniversityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionImages_Competition_CompetitionId",
                table: "CompetitionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExperience_AspNetUsers_UserId",
                table: "PersonalExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_Userld",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_QAA_AspNetUsers_UserID",
                table: "QAA");

            migrationBuilder.DropForeignKey(
                name: "FK_Rule_AspNetUsers_UserID",
                table: "Rule");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorComptiition_Competition_CompetitionID",
                table: "SponsorComptiition");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorComptiition_Sponsor_SponsorID",
                table: "SponsorComptiition");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamCompetition_Competition_CompetitionID",
                table: "TeamCompetition");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamCompetition_Team_TeamId",
                table: "TeamCompetition");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamParticipant_Participant_ParticipantId",
                table: "TeamParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamParticipant_Team_TeamId",
                table: "TeamParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityCompetition_Competition_CompetitionID",
                table: "UniversityCompetition");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityCompetition_University_UniversityId",
                table: "UniversityCompetition");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityImages_University_UniversityId",
                table: "UniversityImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UniversityCompetition",
                table: "UniversityCompetition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_University",
                table: "University");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamParticipant",
                table: "TeamParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamCompetition",
                table: "TeamCompetition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SponsorComptiition",
                table: "SponsorComptiition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sponsor",
                table: "Sponsor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rule",
                table: "Rule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QAA",
                table: "QAA");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalExperience",
                table: "PersonalExperience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participant",
                table: "Participant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Competition",
                table: "Competition");

            migrationBuilder.RenameTable(
                name: "UniversityCompetition",
                newName: "UniversityCompetitions");

            migrationBuilder.RenameTable(
                name: "University",
                newName: "Universities");

            migrationBuilder.RenameTable(
                name: "TeamParticipant",
                newName: "TeamsParticipants");

            migrationBuilder.RenameTable(
                name: "TeamCompetition",
                newName: "TeamsCompetitions");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "SponsorComptiition",
                newName: "SponsorComptiitions");

            migrationBuilder.RenameTable(
                name: "Sponsor",
                newName: "Sponsors");

            migrationBuilder.RenameTable(
                name: "Rule",
                newName: "Rules");

            migrationBuilder.RenameTable(
                name: "QAA",
                newName: "QAAs");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "PersonalExperience",
                newName: "PersonalExperiences");

            migrationBuilder.RenameTable(
                name: "Participant",
                newName: "Participants");

            migrationBuilder.RenameTable(
                name: "Competition",
                newName: "Competitions");

            migrationBuilder.RenameIndex(
                name: "IX_UniversityCompetition_CompetitionID",
                table: "UniversityCompetitions",
                newName: "IX_UniversityCompetitions_CompetitionID");

            migrationBuilder.RenameIndex(
                name: "IX_TeamParticipant_ParticipantId",
                table: "TeamsParticipants",
                newName: "IX_TeamsParticipants_ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamCompetition_CompetitionID",
                table: "TeamsCompetitions",
                newName: "IX_TeamsCompetitions_CompetitionID");

            migrationBuilder.RenameIndex(
                name: "IX_SponsorComptiition_CompetitionID",
                table: "SponsorComptiitions",
                newName: "IX_SponsorComptiitions_CompetitionID");

            migrationBuilder.RenameIndex(
                name: "IX_Rule_UserID",
                table: "Rules",
                newName: "IX_Rules_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_QAA_UserID",
                table: "QAAs",
                newName: "IX_QAAs_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Post_Userld",
                table: "Posts",
                newName: "IX_Posts_Userld");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExperience_UserId",
                table: "PersonalExperiences",
                newName: "IX_PersonalExperiences_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniversityCompetitions",
                table: "UniversityCompetitions",
                columns: new[] { "UniversityId", "CompetitionID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Universities",
                table: "Universities",
                column: "UniversityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamsParticipants",
                table: "TeamsParticipants",
                columns: new[] { "TeamId", "ParticipantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamsCompetitions",
                table: "TeamsCompetitions",
                columns: new[] { "TeamId", "CompetitionID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SponsorComptiitions",
                table: "SponsorComptiitions",
                columns: new[] { "SponsorID", "CompetitionID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sponsors",
                table: "Sponsors",
                column: "SponsorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rules",
                table: "Rules",
                column: "RuleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QAAs",
                table: "QAAs",
                column: "QAAID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalExperiences",
                table: "PersonalExperiences",
                column: "PersonalExperienceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                column: "ParticipantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competitions",
                table: "Competitions",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Universities_UniversityId",
                table: "AspNetUsers",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionImages_Competitions_CompetitionId",
                table: "CompetitionImages",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExperiences_AspNetUsers_UserId",
                table: "PersonalExperiences",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_Userld",
                table: "Posts",
                column: "Userld",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QAAs_AspNetUsers_UserID",
                table: "QAAs",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_AspNetUsers_UserID",
                table: "Rules",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SponsorComptiitions_Competitions_CompetitionID",
                table: "SponsorComptiitions",
                column: "CompetitionID",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SponsorComptiitions_Sponsors_SponsorID",
                table: "SponsorComptiitions",
                column: "SponsorID",
                principalTable: "Sponsors",
                principalColumn: "SponsorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamsCompetitions_Competitions_CompetitionID",
                table: "TeamsCompetitions",
                column: "CompetitionID",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamsCompetitions_Teams_TeamId",
                table: "TeamsCompetitions",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamsParticipants_Participants_ParticipantId",
                table: "TeamsParticipants",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "ParticipantId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamsParticipants_Teams_TeamId",
                table: "TeamsParticipants",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityCompetitions_Competitions_CompetitionID",
                table: "UniversityCompetitions",
                column: "CompetitionID",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityCompetitions_Universities_UniversityId",
                table: "UniversityCompetitions",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityImages_Universities_UniversityId",
                table: "UniversityImages",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Universities_UniversityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionImages_Competitions_CompetitionId",
                table: "CompetitionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExperiences_AspNetUsers_UserId",
                table: "PersonalExperiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_Userld",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_QAAs_AspNetUsers_UserID",
                table: "QAAs");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_AspNetUsers_UserID",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorComptiitions_Competitions_CompetitionID",
                table: "SponsorComptiitions");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorComptiitions_Sponsors_SponsorID",
                table: "SponsorComptiitions");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamsCompetitions_Competitions_CompetitionID",
                table: "TeamsCompetitions");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamsCompetitions_Teams_TeamId",
                table: "TeamsCompetitions");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamsParticipants_Participants_ParticipantId",
                table: "TeamsParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamsParticipants_Teams_TeamId",
                table: "TeamsParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityCompetitions_Competitions_CompetitionID",
                table: "UniversityCompetitions");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityCompetitions_Universities_UniversityId",
                table: "UniversityCompetitions");

            migrationBuilder.DropForeignKey(
                name: "FK_UniversityImages_Universities_UniversityId",
                table: "UniversityImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UniversityCompetitions",
                table: "UniversityCompetitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Universities",
                table: "Universities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamsParticipants",
                table: "TeamsParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamsCompetitions",
                table: "TeamsCompetitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sponsors",
                table: "Sponsors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SponsorComptiitions",
                table: "SponsorComptiitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rules",
                table: "Rules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QAAs",
                table: "QAAs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalExperiences",
                table: "PersonalExperiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Competitions",
                table: "Competitions");

            migrationBuilder.RenameTable(
                name: "UniversityCompetitions",
                newName: "UniversityCompetition");

            migrationBuilder.RenameTable(
                name: "Universities",
                newName: "University");

            migrationBuilder.RenameTable(
                name: "TeamsParticipants",
                newName: "TeamParticipant");

            migrationBuilder.RenameTable(
                name: "TeamsCompetitions",
                newName: "TeamCompetition");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "Sponsors",
                newName: "Sponsor");

            migrationBuilder.RenameTable(
                name: "SponsorComptiitions",
                newName: "SponsorComptiition");

            migrationBuilder.RenameTable(
                name: "Rules",
                newName: "Rule");

            migrationBuilder.RenameTable(
                name: "QAAs",
                newName: "QAA");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "PersonalExperiences",
                newName: "PersonalExperience");

            migrationBuilder.RenameTable(
                name: "Participants",
                newName: "Participant");

            migrationBuilder.RenameTable(
                name: "Competitions",
                newName: "Competition");

            migrationBuilder.RenameIndex(
                name: "IX_UniversityCompetitions_CompetitionID",
                table: "UniversityCompetition",
                newName: "IX_UniversityCompetition_CompetitionID");

            migrationBuilder.RenameIndex(
                name: "IX_TeamsParticipants_ParticipantId",
                table: "TeamParticipant",
                newName: "IX_TeamParticipant_ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamsCompetitions_CompetitionID",
                table: "TeamCompetition",
                newName: "IX_TeamCompetition_CompetitionID");

            migrationBuilder.RenameIndex(
                name: "IX_SponsorComptiitions_CompetitionID",
                table: "SponsorComptiition",
                newName: "IX_SponsorComptiition_CompetitionID");

            migrationBuilder.RenameIndex(
                name: "IX_Rules_UserID",
                table: "Rule",
                newName: "IX_Rule_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_QAAs_UserID",
                table: "QAA",
                newName: "IX_QAA_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_Userld",
                table: "Post",
                newName: "IX_Post_Userld");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExperiences_UserId",
                table: "PersonalExperience",
                newName: "IX_PersonalExperience_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniversityCompetition",
                table: "UniversityCompetition",
                columns: new[] { "UniversityId", "CompetitionID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_University",
                table: "University",
                column: "UniversityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamParticipant",
                table: "TeamParticipant",
                columns: new[] { "TeamId", "ParticipantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamCompetition",
                table: "TeamCompetition",
                columns: new[] { "TeamId", "CompetitionID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sponsor",
                table: "Sponsor",
                column: "SponsorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SponsorComptiition",
                table: "SponsorComptiition",
                columns: new[] { "SponsorID", "CompetitionID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rule",
                table: "Rule",
                column: "RuleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QAA",
                table: "QAA",
                column: "QAAID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalExperience",
                table: "PersonalExperience",
                column: "PersonalExperienceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participant",
                table: "Participant",
                column: "ParticipantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competition",
                table: "Competition",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_University_UniversityId",
                table: "AspNetUsers",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionImages_Competition_CompetitionId",
                table: "CompetitionImages",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExperience_AspNetUsers_UserId",
                table: "PersonalExperience",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_Userld",
                table: "Post",
                column: "Userld",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QAA_AspNetUsers_UserID",
                table: "QAA",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rule_AspNetUsers_UserID",
                table: "Rule",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SponsorComptiition_Competition_CompetitionID",
                table: "SponsorComptiition",
                column: "CompetitionID",
                principalTable: "Competition",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SponsorComptiition_Sponsor_SponsorID",
                table: "SponsorComptiition",
                column: "SponsorID",
                principalTable: "Sponsor",
                principalColumn: "SponsorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamCompetition_Competition_CompetitionID",
                table: "TeamCompetition",
                column: "CompetitionID",
                principalTable: "Competition",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamCompetition_Team_TeamId",
                table: "TeamCompetition",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamParticipant_Participant_ParticipantId",
                table: "TeamParticipant",
                column: "ParticipantId",
                principalTable: "Participant",
                principalColumn: "ParticipantId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamParticipant_Team_TeamId",
                table: "TeamParticipant",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityCompetition_Competition_CompetitionID",
                table: "UniversityCompetition",
                column: "CompetitionID",
                principalTable: "Competition",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityCompetition_University_UniversityId",
                table: "UniversityCompetition",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityImages_University_UniversityId",
                table: "UniversityImages",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
