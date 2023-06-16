﻿using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSolution.Models;

namespace Wordle.ViewModels
{
    internal class WordleLineViewModel : BindableBase, INavigationAware
    {
        public const string NavParamName = "LineIndex";
        readonly ILogger<WordleLineViewModel> _logger;

        int _lineIndex;
        bool _IsTargetLine;
        AskModel[] _AskModels;

        public bool IsTargetLine
        {
            get => _IsTargetLine;
            set => SetProperty(ref _IsTargetLine, value);
        }
        internal int LineIndex { get; set; }
        public AskModel[] AskModels
        {
            get => _AskModels;
            internal set => SetProperty(ref _AskModels, value);
        }

        public WordleLineViewModel(ILogger<WordleLineViewModel> logger)
        {
            _logger = logger;
        }

        public void PullCharacter()
        {
            AskModel? targetModel = AskModels.LastOrDefault (am => am.Character != '_');
            if (targetModel is not null)
                targetModel.Character = ' ';
        }

        public void PushCharacter(char character)
        {
            if(!Char.IsLetter(character))
            {
                _logger.Log(LogLevel.Warning, $"NotLetter: {nameof(character)}");
                return;
            }

            AskModel? targetModel = AskModels.FirstOrDefault(am => am.Character == '_');
            if (targetModel is not null)
                targetModel.Character = Char.ToUpper(character);
        }

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext) => IsTargetLine = true;
        public void OnNavigatedFrom(NavigationContext navigationContext) => IsTargetLine = false;
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue(WordleLineViewModel.NavParamName, out int lineIndex))
                return LineIndex == lineIndex;

            return false;
        }
        #endregion
    }
}