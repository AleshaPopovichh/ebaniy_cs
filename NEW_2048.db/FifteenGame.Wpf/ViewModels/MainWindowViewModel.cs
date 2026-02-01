using FifteenGame.Common.BusinessModels;
using FifteenGame.Common.Definitions;
using FifteenGame.Common.Infrastructure;
using FifteenGame.Common.Repositories;
using FifteenGame.Common.Services;
using Ninject;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FifteenGame.Wpf.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IGameService _gameService = NinjectKernel.Instance.Get<IGameService>();
        private readonly IGameRepository _gameRepository = NinjectKernel.Instance.Get<IGameRepository>();

        private GameModel _model = new GameModel();

        private UserModel _user;
        private int _bestScore;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CellViewModel> Cells { get; set; } = new ObservableCollection<CellViewModel>();

        public string UserName => _user?.Name ?? "<нет>";

        public int MoveCount => _model?.MoveCount ?? 0;

        public int Score => _model?.Score ?? 0;

        public int BestScore => _bestScore;

        public MainWindowViewModel()
        {
            Initialize();
        }

        public void SetUser(UserModel user)
        {
            _user = user;
            OnPropertyChanged(nameof(UserName));

            LoadOrCreateGame();
        }

        public void Initialize()
        {
            _model = new GameModel();
            _gameService.Initialize(_model);
            FromModel(_model);
        }

        public void ReInitialize()
        {
            StartNewGame();
        }

        public void MakeMove(MoveDirection direction, Action gameFinishAction)
        {
            if (_user == null)
            {
                return;
            }

            if (!_gameService.MakeMove(_model, direction))
            {
                return;
            }

            _model.MoveCount++;
            _gameRepository.Update(ToDto(_model));
            UpdateBestScore();
            FromModel(_model);

            if (_gameService.Has2048(_model) || !_gameService.HasAnyMoves(_model))
            {
                gameFinishAction?.Invoke();
            }
        }

        private void LoadOrCreateGame()
        {
            var saved = _gameRepository.GetLastByUserId(_user.Id);
            if (saved == null)
            {
                StartNewGame();
                return;
            }

            _model = FromDto(saved);
            _bestScore = _gameRepository.GetBestScoreByUserId(_user.Id);
            OnPropertyChanged(nameof(BestScore));
            FromModel(_model);
        }

        private void StartNewGame()
        {
            _model = new GameModel
            {
                UserId = _user?.Id ?? 0
            };
            _gameService.Initialize(_model);

            if (_user != null)
            {
                _model.Id = _gameRepository.Create(ToDto(_model));
                _bestScore = _gameRepository.GetBestScoreByUserId(_user.Id);
                OnPropertyChanged(nameof(BestScore));
            }

            FromModel(_model);
        }

        private void UpdateBestScore()
        {
            if (_model.Score > _bestScore)
            {
                _bestScore = _model.Score;
                OnPropertyChanged(nameof(BestScore));
            }
        }

        private static GameModel FromDto(FifteenGame.Common.Dto.GameDto dto)
        {
            var model = new GameModel
            {
                Id = dto.Id,
                UserId = dto.UserId,
                MoveCount = dto.MoveCount,
                Score = dto.Score
            };
            model.SetGrid(dto.Cells);
            return model;
        }

        private static FifteenGame.Common.Dto.GameDto ToDto(GameModel model)
        {
            var dto = new FifteenGame.Common.Dto.GameDto
            {
                Id = model.Id,
                UserId = model.UserId,
                MoveCount = model.MoveCount,
                Score = model.Score
            };
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    dto.Cells[row, column] = model[row, column];
                }
            }
            return dto;
        }

        private void FromModel(GameModel model)
        {
            Cells.Clear();
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    Cells.Add(new CellViewModel
                    {
                        Row = row,
                        Column = column,
                        Num = model[row, column]
                    });
                }
            }

            OnPropertyChanged(nameof(MoveCount));
            OnPropertyChanged(nameof(Score));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
