using Microsoft.EntityFrameworkCore;

public class CategoryService
{
    // khai báo context
    private readonly DBContextUser context;

    public CategoryService(DBContextUser contextUser)
    {
        this.context = contextUser;
    }
    // trẩ về danh sách category với bất đồng bộ
    public async Task<IList<Category>> GetCategories()
    {
        return await context.Category.ToListAsync();
    }
    //Thêm mới 1 category theo phương thức bất đồng bộ trả về số lượng dòng được thêm vào
    public async Task<int> AddCategory(string CategoryName)
    {
        Category category = new Category
        {
            CategoryName = CategoryName
        };
        await context.Category.AddAsync(category);
        int numberOfRecordsAdded = await context.SaveChangesAsync();
        return numberOfRecordsAdded;
    }
}