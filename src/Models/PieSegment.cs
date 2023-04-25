namespace TmpSvgProject.Models;

using System.Drawing;

public class PieSegment
{
    public PieSegment(string? label, Color labelColor, double percentage, Color backgroundColor, string? title, string? href) => (Label, LabelColor, Percentage, BackgroundColor, Title, Href) = (label, labelColor, percentage, backgroundColor, title, href);

    public double Percentage { get; set; }

    public Color BackgroundColor { get; set; }

    public string? Label { get; set; }

    public Color LabelColor { get; set; }

    public string? Href { get; set; }

    public string? Title { get; set; }
}