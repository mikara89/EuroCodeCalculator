using System.Collections.ObjectModel;

namespace SectionDesign
{
    public class ListOfLikeBars : ObservableCollection<LinkBar>
    {
        public double GetSumVh() 
        {
            double result=0;
            foreach (var item in this)
            {
                result =+ item.Vh;
            }
            return result;
        }
    }
}
