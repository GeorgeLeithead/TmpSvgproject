namespace TmpSvgProject.ViewComponents;

[ViewComponent(Name = "SvgPie")]
public class SvgPieViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(int width, int height, List<PieSegment> segments, string? style)
    {
        string svgRaw = DashboardHelper.GeneratePieChart(width, height, segments, style);
        return View("SvgPie", svgRaw);
    }
}