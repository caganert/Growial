using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeatherManager : SingletonMonobehaviour<WeatherManager>
{
    public Weather currentWeather = Weather.raining;
    private List<object> weatherChanceList = new List<object>()
    {
                new {
                        SeasonName = Season.Winter,
                        WeatherChance =  new List<object>() {
                                       new { WeatherCondition = Weather.raining, value = 20},
                                       new { WeatherCondition = Weather.sunny, value = 20},
                                       new { WeatherCondition = Weather.snowing, value = 60},
                        }
                    },
                new {
                        SeasonName = Season.Spring,
                        WeatherChance =  new List<object>() {
                                       new { WeatherCondition = Weather.raining, value = 40},
                                       new { WeatherCondition = Weather.sunny, value = 55},
                                       new { WeatherCondition = Weather.snowing, value = 5},
                        }
                    },
                new {
                        SeasonName = Season.Summer,
                        WeatherChance =  new List<object>() {
                                       new { WeatherCondition = Weather.raining, value = 15},
                                       new { WeatherCondition = Weather.sunny, value = 85},
                                       new { WeatherCondition = Weather.snowing, value = 0},
                        }
                    },
                new {
                        SeasonName = Season.Autumn,
                        WeatherChance =  new List<object>() {
                                       new { WeatherCondition = Weather.raining, value = 60},
                                       new { WeatherCondition = Weather.sunny, value = 35},
                                       new { WeatherCondition = Weather.snowing, value = 5},
                        }
                    },
    };

    public ParticleSystem rain;
    public ParticleSystem snow;
    
    //public void SetWeatherConditionsOfTheDay()
    //{
    //    Season currentSeason = TimeManager.Instance.GetGameSeason();
    //    object chance = weatherChanceList.Where( weatherChance => weatherChance == currentSeason).FirstOrDefault();
    //}

    public void ChangeWeather(Season seasonType, Weather weatherType)
    {
        if (weatherType != this.currentWeather)
        {
            this.currentWeather = weatherType;
            this.snow.Stop();
            this.rain.Stop();
            switch (seasonType)
            {
                case Season.Summer:
                    switch (weatherType)
                    {
                        case Weather.raining:
                            this.rain.Play();
                            break;
                        case Weather.snowing:
                            this.snow.Play();
                            break;
                        case Weather.sunny:

                            break;
                        case Weather.none:
                            break;
                        case Weather.count:
                            break;
                    }
                    break;
                case Season.Spring:
                    switch (weatherType)
                    {
                        case Weather.raining:
                            this.rain.Play();
                            break;
                        case Weather.snowing:
                            this.snow.Play();
                            break;
                        case Weather.sunny:
                            break;
                        case Weather.none:
                            break;
                        case Weather.count:
                            break;
                    }
                    break;
                case Season.Autumn:
                    switch (weatherType)
                    {
                        case Weather.raining:
                            this.rain.Play();
                            break;
                        case Weather.snowing:
                            this.snow.Play();
                            break;
                        case Weather.sunny:
                            break;
                        case Weather.none:
                            break;
                        case Weather.count:
                            break;
                    }
                    break;
                case Season.Winter:
                    switch (weatherType)
                    {
                        case Weather.raining:
                            this.rain.Play();
                            break;
                        case Weather.snowing:
                            this.snow.Play();
                            break;
                        case Weather.sunny:
                            break;
                        case Weather.none:
                            break;
                        case Weather.count:
                            break;
                    }
                    break;
                case Season.none:
                    break;
                case Season.count:
                    break;
                default:
                    break;
            }
            GameManager.Instance.currentWeather = this.currentWeather;
        }
    }

    public void ChangeWeatherAccordingToTheSeason()
    {
        int weatherNumber = Generator.Instance.RandomNumber(0, 3);
        ChangeWeather(TimeManager.Instance.GetGameSeason(), (Weather)weatherNumber);
    }

    private void Update()
    {
        ChangeWeatherAccordingToTheSeason();
    }
}
