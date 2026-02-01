using FifteenGame.Common.Dto;
using System.Collections.Generic;

namespace FifteenGame.Common.Repositories
{
    public interface IGameRepository
    {
        GameDto GetByGameId(int gameId);
        IEnumerable<GameDto> GetByUserId(int userId);

        GameDto GetLastByUserId(int userId);
        int GetBestScoreByUserId(int userId);

        int Create(GameDto dto);
        void Update(GameDto dto);
    }
}
