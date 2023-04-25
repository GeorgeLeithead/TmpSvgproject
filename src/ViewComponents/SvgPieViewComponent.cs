namespace TmpSvgProject.ViewComponents;

[ViewComponent(Name = "SvgPie")]
public class SvgPieViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(int width, int height, List<PieSegment> segments, int borderWidth, Color borderColor, int fontSize, string? style)
    {
        var svgRaw = SvgPieHelper.GeneratePieChart(width, height, segments, borderWidth, borderColor, style, fontSize);
        return View("SvgPie", svgRaw);
    }
}
