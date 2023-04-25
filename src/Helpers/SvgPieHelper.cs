namespace TmpSvgProject.Helpers;

public static class SvgPieHelper
{
	public static string GeneratePieChart(int width, int height, List<PieSegment> segments, int borderWidth, Color borderColor, string? styleClass, int fontSize = 16)
	{
		// Generate the SVG code for the pie chart
		StringBuilder svgBuilder = new();
		_ = svgBuilder.Append("<svg viewBox=\"0 0 ").Append(width).Append(' ').Append(height).Append("\" class=\"").Append(styleClass).AppendLine("\" xmlns=\"http://www.w3.org/2000/svg\">");
		var segmentTotalPercent = segments.Sum(s => s.Percentage);
		if (segmentTotalPercent < 100)
		{
			segments.Add(new PieSegment(null, Color.White, 100 - segmentTotalPercent, Color.White, null, null));
		}

		_ = svgBuilder.AppendLine(GeneratePieChartSegments(width, height, segments, borderWidth, borderColor, fontSize));
		_ = svgBuilder.AppendLine("</svg>");

		return svgBuilder.ToString();
	}

	public static string GeneratePieChart(int width, int height, List<PieSegment> segments) => GeneratePieChart(width, height, segments, 1, Color.Black, null, 16);

	public static string GeneratePieChart(int width, int height, List<PieSegment> segments, string? styleClass) => GeneratePieChart(width, height, segments, 1, Color.Black, styleClass, 16);

	private static string GeneratePieChartSegments(int width, int height, List<PieSegment> segments, int borderWidth, Color borderColor, int fontSize)
	{
		width -= (borderWidth * 2);
		height -= (borderWidth * 2);

		// Calculate the center of the pie chart
		var centerX = ((double)width / 2) + borderWidth;
		var centerY = ((double)height / 2) + borderWidth;

		// Calculate the radius of the pie chart
		var radius = (double)Math.Min(width, height) / 2;

		// Generate the SVG paths for each segment
		StringBuilder pathsBuilder = new();
		double startAngle = 0;
		foreach (PieSegment segment in segments)
		{
			// Calculate the end angle of the segment
			var endAngle = startAngle + (segment.Percentage * 3.6f);
			var labelAngle = startAngle + ((endAngle - startAngle) / 2);
			var labelX = centerX + (0.7f * radius * Math.Sin(Math.PI * labelAngle / 180));
			var labelY = centerY - (0.7f * radius * Math.Cos(Math.PI * labelAngle / 180));

			// Generate the SVG path for the segment
			var largeArc = (segment.Percentage > 50) ? 1 : 0;
			if (string.IsNullOrWhiteSpace(segment.Href))
			{
				_ = pathsBuilder.Append("<g>");
			}
			else
			{
				_ = pathsBuilder.Append("<a href=\"").Append(segment.Href).AppendLine("\">");
			}

			if (segments.Count > 1)
			{
				_ = pathsBuilder.Append("<path d=\"M ").Append(centerX).Append(',').Append(centerY).Append(" L ").Append(centerX + (radius * Math.Sin(Math.PI * startAngle / 180))).Append(',').Append(centerY - (radius * Math.Cos(Math.PI * startAngle / 180))).Append(" A ").Append(radius).Append(',').Append(radius).Append(" 0 ").Append(largeArc).Append(",1 ").Append(centerX + (radius * Math.Sin(Math.PI * endAngle / 180))).Append(',').Append(centerY - (radius * Math.Cos(Math.PI * endAngle / 180))).Append(" Z\" fill=\"").Append(ColorTranslator.ToHtml(segment.BackgroundColor)).Append("\" stroke=\"").Append(ColorTranslator.ToHtml(borderColor)).Append("\" stroke-width=\"").Append(borderWidth).AppendLine("\" />");
				if (!string.IsNullOrWhiteSpace(segment.Title))
				{
					_ = pathsBuilder.Append("<title>").Append(segment.Title).Append("</title>");
				}
			}
			else
			{
				_ = pathsBuilder.Append("<circle cx=\"").Append(centerX).Append("\" cy=\"").Append(centerY).Append("\" r=\"").Append(radius).Append("\" fill=\"").Append(ColorTranslator.ToHtml(segment.BackgroundColor)).Append("\" stroke=\"").Append(ColorTranslator.ToHtml(borderColor)).Append("\" stroke-width=\"").Append(borderWidth).AppendLine("\" />");
				labelX = centerX;
				labelY = centerY + (fontSize / Math.PI);
			}

			if (!string.IsNullOrWhiteSpace(segment.Label))
			{
				_ = pathsBuilder.Append("<text x=\"").Append(labelX).Append("\" y=\"").Append(labelY).Append("\" font-size=\"").Append(fontSize).Append("\" text-anchor=\"middle\" fill=\"").Append(ColorTranslator.ToHtml(segment.LabelColor)).Append("\">").Append(segment.Label).AppendLine("</text>");
			}

			if (string.IsNullOrWhiteSpace(segment.Href))
			{
				_ = pathsBuilder.Append("</g>");
			}
			else
			{
				_ = pathsBuilder.Append("</a>");
			}

			// Update the start angle for the next segment
			startAngle = endAngle;
		}

		return pathsBuilder.ToString();
	}
}
