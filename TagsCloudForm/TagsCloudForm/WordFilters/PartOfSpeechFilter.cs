﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenNLP.Tools.PosTagger;
using TagsCloudForm.CircularCloudLayouterSettings;
using TagsCloudForm.Common;

namespace TagsCloudForm.WordFilters
{
    public class PartOfSpeechFilter : IWordsFilter
    {
        public Result<IEnumerable<string>> Filter(HashSet<string> partOfSpeechToFilter, IEnumerable<string> words)
        {
            var modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\", "EnglishPOS.nbin");
            var tagDictDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\", "tagdict");
            var posTagger = new EnglishMaximumEntropyPosTagger(modelPath, tagDictDir);
            var outList = new List<string>();
            foreach (var word in words)
            {
                var speechPart = posTagger.Tag(new[] { word });
                if (!partOfSpeechToFilter.Contains(speechPart[0]))
                    outList.Add(word);
            }
            return Result.Ok((IEnumerable<string>)outList);
        }


        public Result<IEnumerable<string>> Filter(ICircularCloudLayouterWithWordsSettings settings, IEnumerable<string> words)
        {
            HashSet<string> partOfSpeechToFilter;
            try
            {
                var settingsFilename = settings.PartOfSpeechToFilterFile;
                partOfSpeechToFilter = File.ReadAllLines(settingsFilename).ToHashSet(StringComparer.OrdinalIgnoreCase);
            }
            catch (FileNotFoundException)
            {
                return new Result<IEnumerable<string>>("Не удалось загрузить файл с part of speech to filter: файл отсутствует", words);
            }
            catch (Exception e)
            {
                return new Result<IEnumerable<string>>("Не удалось загрузить файл с part of speech to filter: "+e.Message, words);
            }

            return Filter(partOfSpeechToFilter, words);
        }
    }
}
