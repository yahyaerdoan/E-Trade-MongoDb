using AutoMapper;
using MongoDb.UserInterface.Dtos.CategoryDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.CategoryServices;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;
using MongoDB.Driver;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace MongoDb.UserInterface.Services.Concretions.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        #region old version for db connection
        #region Db Settings For Services. This is the Murat teacher's configuration. This is not using. Updated.
        //private readonly IMongoCollection<Category> _categoryCollection;
        #endregion
        #region Db Settings For Services. This is the Murat teacher's configuration. This is not using. Updated.
        //public CategoryService(IMapper mapper, IDatabaseSettings _databaseSettings)
        //{
        //    var client = new MongoClient(_databaseSettings.ConnectionString);
        //    var database = client.GetDatabase(_databaseSettings.DatabaseName);
        //    _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
        //    _mapper = mapper;
        //}
        #endregion
        #endregion

        private readonly IMongoDbContext _mongoDbContext;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IMongoDbContext mongoDbContext)
        {
            _mapper = mapper;
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var values = _mapper.Map<Category>(createCategoryDto);
            await _mongoDbContext.Categories.InsertOneAsync(values);
        }            

        public async Task DeleteCategoryAsync(string id)
        {
            await _mongoDbContext.Categories.DeleteOneAsync(x=> x.Id == id);
        }

        public async Task<List<ResultCategoryDto>> GetAllAsync()
        {
            var values = await _mongoDbContext.Categories.Find(x=> true).SortBy(x=> x.Name).ToListAsync();
            return _mapper.Map<List<ResultCategoryDto>>(values);
        }

        public async Task<GetByIdCategoryDto> GetByIdCategoryAsync(string id)
        {
            var values  = await _mongoDbContext.Categories.Find<Category>(x=> x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdCategoryDto>(values);
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var values = _mapper.Map<Category>(updateCategoryDto);
            await _mongoDbContext.Categories.FindOneAndReplaceAsync(x=> x.Id == updateCategoryDto.Id, values);
        }

        public async Task<byte[]> CreateCategoryListPdfAsync(List<ResultCategoryDto> categories)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().AlignCenter()
                    .Text("Category List")
                    .SemiBold().FontSize(16).FontColor(Colors.Black);

                    page.Content()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("No");
                            header.Cell().Element(CellStyle).Text("Category Id");
                            header.Cell().Element(CellStyle).Text("Name");
                            header.Cell().Element(CellStyle).Text("Description");

                            
                        });

                        int numberOfData = 0;
                        foreach (var category in categories)
                        {
                            numberOfData++;
                            table.Cell().Element(CellStyle).Text(numberOfData.ToString());
                            table.Cell().Element(CellStyle).Text(category.Id);
                            table.Cell().Element(CellStyle).Text(category.Name);
                            table.Cell().Element(CellStyle).Text(category.Description);
                        }
                        static IContainer CellStyle(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        }
                    });


                    page.Footer()
                        .Column(column =>
                        {
                            column.Item().AlignCenter().Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                                x.Span(" of ");
                                x.TotalPages();
                            });
                            column.Item().AlignCenter().Text("Yahya John Erdogan").FontSize(10).FontColor(Colors.Black);
                        });
                });
            });
            return await Task.Run(() => document.GeneratePdf()) ;
        }

        public async Task<byte[]> CreateCategoryListExcelAsync(List<ResultCategoryDto> categories)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Categories");

                // Add headers
                worksheet.Cells[1, 1].Value = "Number Of Category";
                worksheet.Cells[1, 2].Value = "Category Id";
                worksheet.Cells[1, 3].Value = "Name";
                worksheet.Cells[1, 4].Value = "Description";

                // Add data
                int numberOfData = 0;
                for (int i = 0; i < categories.Count; i++)
                {
                    numberOfData++;
                    worksheet.Cells[i + 2, 1].Value = numberOfData;
                    worksheet.Cells[i + 2, 2].Value = categories[i].Id;
                    worksheet.Cells[i + 2, 3].Value = categories[i].Name;
                    worksheet.Cells[i + 2, 4].Value = categories[i].Description;
                }

                // Format the header
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                return await Task.Run(()=> package.GetAsByteArray());
            }
        }
    }
}
