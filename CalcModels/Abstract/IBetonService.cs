namespace CalcModels
{
    public interface IBetonService
    {
        IBetonModel[] GetList();
        string[] GetNameList();
        IBetonModel GetNew(string name);
    }
}