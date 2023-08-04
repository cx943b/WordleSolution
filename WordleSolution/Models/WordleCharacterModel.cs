using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Wordle.Models
{
    public class WordleCharacterModel : BindableBase
    {
        char _Character = '_';
        bool _IsInputTarget;
        bool _IsExisted;
        bool _IsCurrected;
        bool _IsExepted;

        public bool IsInputTarget
        {
            get => _IsInputTarget;
            set => SetProperty(ref _IsInputTarget, value);
        }
        public char Character
        {
            get => _Character;
            set => SetProperty(ref _Character, value);
        }
        public bool IsExisted
        {
            get => _IsExisted;
            set => SetProperty(ref _IsExisted, value);
        }
        public bool IsCurrected
        {
            get => _IsCurrected;
            set => SetProperty(ref _IsCurrected, value);
        }
        public bool IsExepted
        {
            get => _IsExepted;
            set => SetProperty(ref _IsExepted, value);
        }
    }

    public class WordleLineCharacterModel : WordleCharacterModel
    {
        char _befDragChar = '_';

        bool _IsDropTarget;
        bool _IsPrinted;

        DelegateCommand<DragEventArgs> _DragEnterCommand;
        DelegateCommand<DragEventArgs> _DragLeaveCommand;
        DelegateCommand<DragEventArgs> _DropCommand;

        public WordleCharacterModel? DropedCharModel { get; set; }

        public bool IsDropTarget
        {
            get => _IsDropTarget;
            set => SetProperty(ref _IsDropTarget, value);
        }
        public bool IsPrinted
        {
            get => _IsPrinted;
            set => SetProperty(ref _IsPrinted, value);
        }

        public ICommand DragEnter => _DragEnterCommand;
        public ICommand DragLeave => _DragLeaveCommand;
        public ICommand Drop => _DropCommand;


        public WordleLineCharacterModel()
        {
            _DragEnterCommand = new DelegateCommand<DragEventArgs>(onDragEnter, e => !_IsPrinted).ObservesProperty(() => IsPrinted);
            _DragLeaveCommand = new DelegateCommand<DragEventArgs>(onDragLeave, e => !_IsPrinted).ObservesProperty(() => IsPrinted);
            _DropCommand = new DelegateCommand<DragEventArgs>(onDrop, e => !_IsPrinted).ObservesProperty(() => IsPrinted);
        }

        private void onDragEnter(DragEventArgs e)
        {
            if (IsCurrected)
                return;

            _befDragChar = Character;

            bool isValidData = tryGetData(e.Data, out WordleCharacterModel? askModel);
            if (isValidData && askModel is not null)
            {
                IsDropTarget = true;
                Character = askModel.Character;
            }
        }
        private void onDragLeave(DragEventArgs e)
        {
            if (IsCurrected)
                return;

            IsDropTarget = false;

            if(isValidateData(e.Data))
                Character = _befDragChar;
        }
        private void onDrop(DragEventArgs e)
        {
            if (IsCurrected)
                return;

            IsDropTarget = false;

            bool isValidData = tryGetData(e.Data, out WordleCharacterModel? askModel);
            if (isValidData && askModel is not null)
            {
                DropedCharModel = e.Data.GetData(typeof(WordleCharacterModel)) as WordleCharacterModel;
            }
        }

        private bool isValidateData(IDataObject dragData) => dragData.GetDataPresent(typeof(WordleCharacterModel));

        private bool tryGetData(IDataObject dragData, out WordleCharacterModel? askModel)
        {
            if (isValidateData(dragData))
            {
                askModel = dragData.GetData(typeof(WordleCharacterModel)) as WordleCharacterModel;
                return true;
            }

            askModel = null;
            return false;
        }
    }
}
