#include "lib/Environment.jsxinc";
#include "lib/Extractor.jsxinc";
#include "lib/Parser.jsxinc";
#include "lib/ImageExporter.jsxinc";
#include "lib/JsonFileWriter.jsxinc";
#include "lib/XmlFileWriter.jsxinc";

const PSD_FILE_NAME_PATTERN = /(psd)$/ig;

app.preferences.rulerUnits = Units.PIXELS;
app.preferences.typeUnits = TypeUnits.PIXELS;

function getPsdFileList()
{
	var folder = new Folder(String(app.activeDocument.path));
	var result = new Array();
	var files = folder.getFiles();
	for(var i = 0; i < files.length; i++)
	{
		if(files[i].name.match(PSD_FILE_NAME_PATTERN) != null)
		{
			result.push(files[i]);
		}
	}
	return result;
}

function main()
{
	var t = new Date().getTime();
	var fileList = getPsdFileList();
	for(var i = 0; i < fileList.length; i++)
	{
		app.open(fileList[i]);
		
		var env = new Environment(app.activeDocument);
		var extractData = new Extractor(app.activeDocument).extract();
		var jsonFileWriter = new JsonFileWriter(env);
		//jsonFileWriter.writeExtractResult(extractData);

		var parser = new Parser(env);
		parser.loadSharedAssetXml();
		var parseData = parser.parse(extractData);
		jsonFileWriter.writeParseResult(parseData);
		var xmlFileWriter = new XmlFileWriter(env);
		xmlFileWriter.writeAssetXml(parser.assetMap);
		/*
		var imageExporter = new ImageExporter(env);
		imageExporter.export(extractData);
		*/
		app.activeDocument.close(SaveOptions.DONOTSAVECHANGES);
	}
	alert("DONE! 耗时" + (new Date().getTime() - t) * 0.001 + "秒");
}

main();