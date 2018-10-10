#include "lib/Environment.jsxinc";
#include "lib/Extractor.jsxinc";
#include "lib/Parser.jsxinc";
#include "lib/ImageExporter.jsxinc";
#include "lib/JsonFileWriter.jsxinc";
#include "lib/XmlFileWriter.jsxinc";


app.preferences.rulerUnits = Units.PIXELS;
app.preferences.typeUnits = TypeUnits.PIXELS;

function main()
{
	var t = new Date().getTime();
	var env = new Environment(app.activeDocument);
	var waring = [];
	var extractData = new Extractor(app.activeDocument).extract(waring);
	var jsonFileWriter = new JsonFileWriter(env);
	//jsonFileWriter.writeExtractResult(extractData);

	var parser = new Parser(env);
	parser.loadSharedAssetXml();
	var parseData = parser.parse(extractData);
	jsonFileWriter.writeParseResult(parseData);
	var xmlFileWriter = new XmlFileWriter(env);
	xmlFileWriter.writeAssetXml(parser.assetMap);
	var imageExporter = new ImageExporter(env);
	imageExporter.export(extractData);
	if(waring !=null && waring.length > 0)
	{
		var log = "以下文本头尾含有换行符或空格，如果不是预期效果，请修正：";
		for(var i = 0; i < waring.length; i++)
		{
			log = log + "\n" + waring[i];
		}
		alert(log);
	}
	else
	{
		alert("DONE! 耗时" + (new Date().getTime() - t) * 0.001 + "秒");
	}
}

main();