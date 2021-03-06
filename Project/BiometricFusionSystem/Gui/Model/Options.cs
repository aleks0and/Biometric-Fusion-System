﻿using Gui.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Model
{
    public class Options : BindableBase
    {
        private AcquisitionMethod _acquisitionMethod;
        public AcquisitionMethod AcquisitionMethod
        {
            get { return _acquisitionMethod; }
            set { _acquisitionMethod = value; Notify(); }
        }

        private SilenceRemoval _silenceRemoval;
        public SilenceRemoval SilenceRemoval
        {
            get { return _silenceRemoval; }
            set { _silenceRemoval = value; Notify(); }
        }

        private VerificationMethod _verificationMethod;
        public VerificationMethod VerificationMethod
        {
            get { return _verificationMethod; }
            set { _verificationMethod = value; Notify(); }
        }

        private IdentificationMethod _identificationMethod;
        public IdentificationMethod IdentificationMethod
        {
            get { return _identificationMethod; }
            set { _identificationMethod = value; Notify(); }
        }
        private ObservableCollection<string> _words;
        public ObservableCollection<string> Words
        {
            get { return _words; }
            set { _words = value; Notify(); }
        }
        private string _currentWord;
        public string CurrentWord
        {
            get { return _currentWord; }
            set { _currentWord = value; Notify(); }
        }
        public Options(AcquisitionMethod acquisition, SilenceRemoval silence, 
            VerificationMethod verification, IdentificationMethod identification)
        {
            AcquisitionMethod = acquisition;
            SilenceRemoval = silence;
            VerificationMethod = verification;
            IdentificationMethod = identification;
        }
    }
}
