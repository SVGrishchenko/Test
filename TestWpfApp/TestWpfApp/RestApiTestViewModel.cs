using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml;

namespace TestWpfApp
{
    public class RestApiTestViewModel : ObservableObject
    {
        private const string _link = "https://beacon.nist.gov/rest/record/";

        private ObservableCollection<Symbol> _symbols;
        private RelayCommand _runCommand;
        private DateTime _timePick;
        private string _timePickerString;
        private string _timeStmp;
        private DateTime _datePick = DateTime.Now;
        private string _outputValue;
        private IXmlReader _xmlReader;

        public RestApiTestViewModel(IXmlReader xmlReader)
        {
            _xmlReader = xmlReader;
            DatePick = DateTime.Now;
            Symbols = new ObservableCollection<Symbol>();
        }

        public DateTime TimePick
        {
            get
            {
                return _timePick;
            }
            set
            {
                _timePick = value;
                Set(ref _timeStmp, GetTimeStamp());
                
            }
        }

        public string TimePickerString
        {
            get
            {
                return _timePickerString;
            }
            set
            {
                _timePickerString = value;
            }
        }

        public DateTime DatePick
        {
            get
            {
                return _datePick;
            }
            set
            {
                _datePick = value;
                TimeStamp = GetTimeStamp();
                RaisePropertyChanged("DatePick");
            }
        }

        public string TimeStamp
        {
            get
            {
                return _timeStmp;
            }
            set
            {
                _timeStmp = value;
                Symbols.Clear();
                OutputValue = "";
                RaisePropertyChanged("TimeStamp");
             }
        }

        public string OutputValue
        {
            get
            {
                return _outputValue;
            }
            set
            {
                _outputValue = value;
                RaisePropertyChanged("OutputValue");
            }
        }


        public RelayCommand RunCommand
        {
            get
            {
                return _runCommand ?? (_runCommand = new RelayCommand(Run));
            }
        }



        public ObservableCollection<Symbol> Symbols
        {
            get
            {
                return _symbols ?? (_symbols = new ObservableCollection<Symbol>());
            }
            set
            {
                Set(ref _symbols, value);
            }
        }


        private string GetTimeStamp()
        {
            Int32 unixTimestamp = (Int32)(_datePick.Subtract(new DateTime(1970, 1, 1))).TotalSeconds / 100;
            return unixTimestamp.ToString();
        }

        public void Run()
        {
            OutputValue = "";
            Symbols.Clear(); 

            string link = _link + TimeStamp;

            try
            {
                OutputValue = _xmlReader.GetValue(link, "outputValue");

                if (!String.IsNullOrEmpty(OutputValue))
                {
                    var list = OutputValue.ToArray()
                        .GroupBy(x => x)
                        .Select(grp => new Symbol { Char = grp.Key, Count = grp.Count() })
                        .OrderBy(g => g.Char)
                        .ToList();
                    foreach (Symbol s in list)
                    {
                        Symbols.Add(s);
                    }
                }
            }
            catch(Exception ex)
            {
                OutputValue = ex.Message;
            }
        }

    }
}
