namespace Educal_MVC.Helpers
{
    public class Paginate<T>
    {
        public IEnumerable<T> Datas { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }

        public bool HasNext
        {
            get
            {
                return CurrentPage < TotalPage;
            }
        }

        public bool HasPrevious
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public Paginate(IEnumerable<T> datas, int totalPage, int currentPage)
        {
            Datas = datas;
            TotalPage = totalPage;
            CurrentPage = currentPage;
        }
    }
}
