namespace TmpSvgProject.Helpers;

public static class DashboardHelper
{
    public static string GeneratePieChart(int width, int height, List<PieSegment> segments)
    {
        return GeneratePieChart(width, height, segments, null);
    }

    public static string GeneratePieChart(int width, int height, List<PieSegment> segments, string? styleClass)
    {
        // Generate the SVG code for the pie chart
        StringBuilder svgBuilder = new();
        _ = svgBuilder.Append("<svg viewBox=\"0 0 ").Append(width).Append(' ').Append(height).Append("\" class=\"").Append(styleClass).AppendLine("\" xmlns=\"http://www.w3.org/2000/svg\">");
        var segmentTotalPercent = segments.Sum(s => s.Percentage);
        if (segmentTotalPercent < 100)
        {
            segments.Add(new PieSegment(null, Color.White, 100 - segmentTotalPercent, Color.White, null, null));
        }

        _ = svgBuilder.AppendLine(GeneratePieChartSegments(width, height, segments, 1, Color.Black));
        _ = svgBuilder.AppendLine("</svg>");

        return svgBuilder.ToString();
    }

    public static string GeneratePieChartSegments(int width, int height, List<PieSegment> segments, int borderWidth, Color borderColor)
    {
        // Calculate the center of the pie chart
        double centerX = (double)width / 2;
        double centerY = (double)height / 2;

        // Calculate the radius of the pie chart
        double radius = (double)Math.Min(width - (borderWidth * 2), height - (borderWidth * 2)) / 2;

        // Generate the SVG paths for each segment
        StringBuilder pathsBuilder = new();
        double startAngle = 0;
        foreach (PieSegment segment in segments)
        {
            // Calculate the end angle of the segment
            double endAngle = startAngle + (segment.Percentage * 3.6f);
            double labelAngle = startAngle + ((endAngle - startAngle) / 2);
            double labelX = centerX + (0.7f * radius * Math.Sin(Math.PI * labelAngle / 180));
            double labelY = centerY - (0.7f * radius * Math.Cos(Math.PI * labelAngle / 180));

            // Generate the SVG path for the segment
            int largeArc = (segment.Percentage > 50) ? 1 : 0;
            if (string.IsNullOrWhiteSpace(segment.Href))
            {
                pathsBuilder.Append("<g>");
            }
            else
            {
                pathsBuilder.Append("<a href=\"").Append(segment.Href).AppendLine("\">");
            }

            if (segments.Count > 1)
            {
                pathsBuilder.Append("<path d=\"M ").Append(centerX).Append(',').Append(centerY).Append(" L ").Append(centerX + (radius * Math.Sin(Math.PI * startAngle / 180))).Append(',').Append(centerY - (radius * Math.Cos(Math.PI * startAngle / 180))).Append(" A ").Append(radius).Append(',').Append(radius).Append(" 0 ").Append(largeArc).Append(",1 ").Append(centerX + (radius * Math.Sin(Math.PI * endAngle / 180))).Append(',').Append(centerY - (radius * Math.Cos(Math.PI * endAngle / 180))).Append(" Z\" fill=\"").Append(ColorTranslator.ToHtml(segment.BackgroundColor)).Append("\" stroke=\"").Append(ColorTranslator.ToHtml(borderColor)).Append("\" stroke-width=\"").Append(borderWidth).AppendLine("\" />");
                if (!string.IsNullOrWhiteSpace(segment.Title))
                {
                    pathsBuilder.Append("<title>").Append(segment.Title).Append("</title>");
                }
            }
            else
            {
                pathsBuilder.Append("<circle cx=\"").Append(centerX).Append("\" cy=\"").Append(centerY).Append("\" r=\"").Append(radius).Append("\" fill=\"").Append(ColorTranslator.ToHtml(segment.BackgroundColor)).Append("\" stroke=\"").Append(ColorTranslator.ToHtml(borderColor)).Append("\" stroke-width=\"").Append(borderWidth).AppendLine("\" />");
                labelX = centerX;
                labelY = centerY * 1.1;
            }

            if (!string.IsNullOrWhiteSpace(segment.Label))
            {
                pathsBuilder.Append("<text x=\"").Append(labelX).Append("\" y=\"").Append(labelY).Append("\" font-size=\"16\" text-anchor=\"middle\" fill=\"").Append(ColorTranslator.ToHtml(segment.LabelColor)).Append("\">").Append(segment.Label).AppendLine("</text>");
            }

            if (string.IsNullOrWhiteSpace(segment.Href))
            {
                pathsBuilder.Append("</g>");
            }
            else
            {
                pathsBuilder.Append("</a>");
            }

            // Update the start angle for the next segment
            startAngle = endAngle;
        }

        return pathsBuilder.ToString();
    }
}