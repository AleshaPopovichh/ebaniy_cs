using FifteenGame.Common.BusinessModels;

namespace FifteenGame.Common.Services
{
    public interface IGameService
    {
        void Initialize(GameModel model);
        void Shuffle(GameModel model);
        bool MakeMove(GameModel model, MoveDirection direction);

        bool HasAnyMoves(GameModel model);
        bool Has2048(GameModel model);
    }
}
