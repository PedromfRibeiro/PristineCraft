namespace PristineCraft.Application.Common.Models;

public class FilterParams
{
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > _maxPagesSize ? _maxPagesSize : value;
    }

    private const int _maxPagesSize = 50;
    public int PageNumber { get; set; } = 1;
    public List<string>? InputOptions { get; set; }

    internal List<FilterOptions>? _filterOptions;

    public List<FilterOptions>? FilterOptions2
    {
        get { return _filterOptions = Filter(InputOptions); }
        set { _filterOptions = value; }
    }

    public List<FilterOptions>? Filter(List<string>? FilterOptions)
    {
        if (FilterOptions != null)
        {
            _filterOptions = new List<FilterOptions> { };
            foreach (var word in FilterOptions)
            {
                string[] words = word.Split('-');
                _filterOptions.Add(new FilterOptions { field = words[0], operators = words[1], value = words[2] });
            }
            return _filterOptions;
        }
        return null;
    }
}