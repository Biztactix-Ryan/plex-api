namespace Plex.Api.Automapper
{
    using ApiModels;
    using PlexModels.Account;
    using Profile = AutoMapper.Profile;

    public class PlexAccountModelMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlexAccountModelMapper"/> class.
        /// </summary>
        public PlexAccountModelMapper()
        {
            CreateMap<PlexAccount, Account>();
        }
    }
}