using CalculatorEC2Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TabeleEC2;
using TabeleEC2.Model;
using Windows.UI.Popups;

namespace TransversalReinf_EC2.ViewModel
{
    public class TransversalReinf_EC2ViewModel:BaseViewModel
    {

        public ICommand CalculateCommand { get; set; }
        public ICommand CalculateArmCommand { get; set; }
        private TransverzalneSileEC2 Transverzal;
        public TransversalReinf_EC2ViewModel(double Vg,double Vq,double b, double h, double d1, ReinforcementTypeModelEC armatura, BetonModelEC beton,ReinforcementModelEC Longitud_As1)
        {
            this.Vg = Vg;
            this.Vq = Vq;
            this.b = b;
            this.h = h;
            this.d1 = d1;
            this.armatura = armatura;
            this.beton = beton;
            this.Longitud_As1 = Longitud_As1;

            CalculateCommand = new CommandHandler(async() =>await Calculate());
            CalculateArmCommand = new CommandHandler(async () => await CalculateArm());

        }

        public async Task Calculate()
        {
            try
            {
                if (Transverzal != null)
                    Transverzal.Dispose();

                if (!IsVedToggled)
                    Transverzal = new TransverzalneSileEC2(b, h, beton, armatura, Longitud_As1, Vg, Vq, d1, 0, 0);
                else Transverzal = new TransverzalneSileEC2(b, h, beton, armatura, Longitud_As1, Ved, d1, 0, 0);

                Errors = new ObservableCollection<string>(Transverzal.Errors);
                IsCalculationValid = Transverzal.Errors.Count() == 0 ? true : false;

                if (IsCalculationValid)
                {
                    Transversal_Asw = new ReinforcementModelEC(ReinforcementType.GetAramturaList().Single(n => n.diameter == 10), 1);

                    ListOfS = new ObservableCollection<double>(Transverzal.List_s);
                    ListOfM = new ObservableCollection<int>(Transverzal.List_m);

                    if (Transverzal.List_m.Min() == m) ++m;
                        m = 2;
                    if (12.5 == s) ++s;
                        s = 12.5;
                    NotUpdated = false;
                    A_add_pot = Transverzal.As_add;
                    A_add_usv= new ReinforcementModelEC(ReinforcementType.GetAramturaList().Single(n => n.diameter == 16), 1);
                    //Transversal_Asw_min = new ReinforcementModel(ReinforcementType.GetAramturaList().Single(n => n.diameter == 6), 1);
                    Asw_min = Transverzal.Asw_min;
                    if (Asw_min > 0)
                    {
                        Transversal_Asw = new ReinforcementModelEC(ReinforcementType.GetAramturaList().Single(n => n.diameter == 6), 1);
                        if (Transverzal.List_m.Min() == m) ++m;
                        m = Transverzal.List_m.Min();
                        if (Transverzal.List_s.Min() == s) ++s;
                        s = Transverzal.s_max;
                    }
                    Result = Transverzal.ToString();
                }
            }
            catch (System.Exception ex)
            {
                var dialoga = new MessageDialog("Greška: " +ex.Message);
                await dialoga.ShowAsync();
                return;
            }
           
        }
        public async Task CalculateArm() 
        {
            if (IsCalculationValid)
            {
                Transverzal.CalculateArmature(m, s, Transversal_Asw);
                IskoriscenostArmature = Transverzal.IskoriscenostArmature;
                IskoriscenostBetona = Transverzal.IskoriscenostBetona;
                Result = Transverzal.ToString();
            }
        }
      

        #region Propertes
        private double _Vg;
        public double Vg
        {
            get { return _Vg; }
            set
            {
                if (value != null || value != _Vg)
                {
                    SetValue(ref _Vg, value);
                    NotUpdated = true;
                }
                    
            }
        }

        private double _Ved=147.75;
        public double Ved
        {
            get { return _Ved; }
            set
            {
                if (value != null || value != _Ved)
                {
                    SetValue(ref _Ved, value);
                    NotUpdated = true;
                }
                    
            }
        }
        private double _Vq;
        public double Vq
        {
            get { return _Vq; }
            set
            {
                if (value != null || value != _Vq)
                {
                    SetValue(ref _Vq, value);
                    NotUpdated = true;
                }
                
            }
        }

        private string _Result;
        public string Result
        {
            get { return _Result; }
            set
            {
                if (value != null || value != _Result)
                    SetValue(ref _Result, value);
            }
        }
        private double _b;
        public double b
        {
            get { return _b; }
            set
            {
                if (value != null || value != _b)
                {
                    SetValue(ref _b, value);
                    NotUpdated = true;
                }               
            }
        }

        private double _h;
        public double h
        {
            get { return _h; }
            set
            {
                if (value != null || value != _h)
                {
                    SetValue(ref _h, value);
                    NotUpdated = true;
                }
            }
        }

        private ReinforcementTypeModelEC _armatura;
        public ReinforcementTypeModelEC armatura
        {
            get { return _armatura; }
            set
            {
                if (value != null || value != _armatura)
                {
                    SetValue(ref _armatura, value);
                    NotUpdated = true;
                }
            }
        }

        private BetonModelEC _beton;
        public BetonModelEC beton
        {
            get { return _beton; }
            set
            {
                if (value != null || value != _beton)
                {
                    SetValue(ref _beton, value);
                    NotUpdated = true;
                }
                
            }
        }

        private ReinforcementModelEC _Longitud_As1;
        public ReinforcementModelEC Longitud_As1
        {
            get { return _Longitud_As1; }
            set
            {
                if (value != null || value != _Longitud_As1)
                {
                    SetValue(ref _Longitud_As1, value);
                    NotUpdated = true;
                }
            }
        }
        private double _d1;
        public double d1
        {
            get { return _d1; }
            set
            {
                if (value != null || value != _d1)
                {
                    SetValue(ref _d1, value);
                    NotUpdated = true;
                }
                
            }
        }


        //private ReinforcementModel _Transversal_Asw_min;
        //public ReinforcementModel Transversal_Asw_min
        //{
        //    get { return _Transversal_Asw_min; }
        //    set
        //    {
        //        if (value != null || value != _Transversal_Asw_min)
        //            SetValue(ref _Transversal_Asw_min, value);
        //    }
        //}

        private ReinforcementModelEC _Transversal_Asw; 
        public ReinforcementModelEC Transversal_Asw
        {
            get { return _Transversal_Asw; }
            set
            {
                if (value != null || value != _Transversal_Asw)
                {
                    SetValue(ref _Transversal_Asw, value);
                    CalculateArm();
                }
            }
        }

        private int _m;
        public int m
        {
            get { return _m; }
            set
            {
                if (value != null || value != _m)
                {
                    SetValue(ref _m, value);
                    CalculateArm();
                }
                    
            }
        }

        private double _s;
        public double s
        {
            get { return _s; }
            set
            {
                if (value != null || value != _s)
                {
                    SetValue(ref _s, value);
                    CalculateArm();
                }
            }
        }

        private bool _IsCalculationValid; 
        public bool IsCalculationValid
        {
            get { return _IsCalculationValid; }
            set
            {
                if (value != null || value != _IsCalculationValid)
                    SetValue(ref _IsCalculationValid, value);
            }
        }
        public List<int> ListOfNumArm {
            get { return new List<int>() {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15 }; }
        }
        public List<ReinforcementTabelModel> ListOfArmDiam
        {
            get { return ReinforcementType.GetAramturaList(); }
        }

        public List<BetonModelEC> ListOfBetonClasses
        {
            get { return BetonClasses.GetBetonClassListEC(); }
        }

        public List<ReinforcementTypeModelEC> ListOfArmatureType 
        {
            get { return ReinforcementType.GetArmatura(); }
        }

        private ObservableCollection<double> _ListOfS;
        public ObservableCollection<double> ListOfS
        {
            get { return _ListOfS; }
            set
            {
                if (value != null || value != _ListOfS)
                    SetValue(ref _ListOfS, value);
            }
        }

        private ObservableCollection<int> _ListOfM;
        public ObservableCollection<int> ListOfM
        {
            get { return _ListOfM; }
            set
            {
                if (value != null || value != _ListOfM)
                    SetValue(ref _ListOfM, value);
            }
        }

        private ObservableCollection<string> _Errors;
        public ObservableCollection<string> Errors 
        {
            get { return _Errors; }
            set
            {
                if (value != null || value != _Errors)
                    SetValue(ref _Errors, value);
            }
        }

        private double _Asw_min;
        public double Asw_min
        {
            get { return Math.Round( _Asw_min,2); }
            set
            {
                if (value != null || value != _Asw_min)
                    SetValue(ref _Asw_min, value);
            }
        }
        private double _IskoriscenostArmature;
        public double IskoriscenostArmature 
        {
            get { return _IskoriscenostArmature*100; }
            set
            {
                if (value != null || value != _IskoriscenostArmature)
                    SetValue(ref _IskoriscenostArmature, value);
            }
        }
        private double _IskoriscenostBetona; 
        public double IskoriscenostBetona
        {
            get { return _IskoriscenostBetona * 100; }
            set
            {
                if (value != null || value != _IskoriscenostBetona)
                    SetValue(ref _IskoriscenostBetona, value);
            }
        }

        private bool _NotUpdated;
        public bool NotUpdated
        {
            get { return _NotUpdated; }
            set
            {
                if (value != null || value != _NotUpdated)
                    SetValue(ref _NotUpdated, value);
            }
        }

        private double _A_add_pot;
        public double A_add_pot
        {
            get { return Math.Round( _A_add_pot,2); }
            set
            {
                if (value != null || value != _A_add_pot)
                    SetValue(ref _A_add_pot, value);
            }
        }

        private ReinforcementModelEC _A_add_usv;
        public ReinforcementModelEC A_add_usv
        {
            get { return _A_add_usv; }
            set
            {
                if (value != null || value != _A_add_usv)
                    SetValue(ref _A_add_usv, value);
            }
        }

        private bool _IsVedToggled=true;
        public bool IsVedToggled
        {
            get { return _IsVedToggled; }
            set
            {
                if (value != null || value != _IsVedToggled)
                {
                    SetValue(ref _IsVedToggled, value);
                    NotUpdated = true;
                }
                
            }
        }
        #endregion



    }
}
