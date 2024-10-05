using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RAZ;

class Program
{
    static void Main(string[] args)
    {
        var content= File.ReadAllText("ouput.txt");
        
        //model
       // {"quizStars":0,"isPlanLocked":false,"id":2814,"readStars":0,"isFreeBook":false,"goldStars":0,"scene":["Others"],"sid":[0],"guest_readable":0,"pkStars":0,"selected":false,"isWords":false,"cat_name":"Z","is_new":true,"read_score_max":0,"number":0,"orientation":2,"interact":false,"zipfile":"https:\/\/qnfile.abctime.com\/bookzf_1555050137.zip","previewStars":0,"has_audio":false,"book_name":"The Queen's Loss (Part I)","themeName":"Others","preview_words":0,"is_finish":false,"themeId":"0","is_lock":false,"isRead":false,"cid":"29","topic":0,"words_num":1818,"isRecordingScore":false,"read_time":29,"pic":"https:\/\/qnfile.abctime.com\/book_1555042035.png","isQuiz":false}

        
        //提取文本中所有json字符
        var jsons = Regex.Matches(content, @"\{.*?\}");
        IList<model> add = new List<model>();
        foreach (Match json in jsons)
        {
            var model = JsonSerializer.Deserialize<model>(json.Value);
            if (model.cat_name=="E")
            {
               add.Add(model);
            }
        }
        
        add=add.OrderBy(x=>x.book_name).ToList();
        
        // mp3 files 
        var mp3files=Directory.GetFiles("RAZ","*.mp3");
        
       var  filename=mp3files.Select(x=>new FileInfo(x).Name.ToUpper().Replace(" ","")).ToArray();
        //index
        for (int i = 0; i < add.Count; i++)
        {
            bool isMatch = false;
            for (var index = 0; index < filename.Length; index++)
            {
               
                var name = filename[index];
                if (name.Contains(add[i].book_name.Replace(",","")
                        .Replace("!","")
                        .Replace("?","")
                        .Replace("'","")
                        .Replace("\"","")
                        .Replace(" ","").ToUpper()))
                {
                   // Console.WriteLine($"{i + 1}-{add[i].book_name} has mp3");
                   new FileInfo(mp3files[index]).CopyTo($"{i + 1} {add[i].book_name}.mp3",true);
                   isMatch = true;

                }
                else
                {
                    // Console.WriteLine($"{i + 1}-{add[i].book_name} has no mp3");
                }
            }

            if (!isMatch)
            {
                Console.WriteLine($"{i + 1}-{add[i].book_name} has no mp3");
            }
        }
        
        Console.WriteLine("Done");
    }
    
    
}