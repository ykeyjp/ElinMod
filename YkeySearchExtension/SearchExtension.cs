using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YkeySearchExtension
{
    public class SearchExtension
    {
        private string[] kw;
        public HashSet<Card> foundCard;
        public bool onlyShop = false;

        public SearchExtension(string keyword)
        {
            foundCard = new HashSet<Card>();
            kw = keyword.Replace("　", " ").Replace("：", ":").Split(' ');
        }

        public void Execute()
        {
            SeachChara();
            if (!onlyShop)
            {
                SearchMap();
            }
        }

        public void Execute(List<Thing> container)
        {
            foreach (var t in container)
            {
                SearchAndAdd(t);
            }
        }

        private void SearchAndAdd(Card card)
        {
            foreach (var _w in kw)
            {
                if (_w.IsEmpty())
                {
                    continue;
                }
                var w = _w.ToLower();

                if (w[0] == ':')
                {
                    if (card is Chara)
                    {
                        return;
                    }

                    var w2 = w.Substring(1);
                    if (card.elements.dict.Values.Where(e => e.FullName.ToLower().Contains(w2) || e.source.GetName().ToLower().Contains(w2)).Count() == 0)
                    {
                        return;
                    }
                }
                else
                {
                    if (!card.Name.ToLower().Contains(w) && !card.sourceCard.GetSearchName(false).Contains(w))
                    {
                        return;
                    }
                }
            }

            foundCard.Add(card);
        }

        private void SeachChara()
        {
            foreach (Chara chara in EMono._map.charas)
            {
                if (!onlyShop)
                {
                    SearchAndAdd(chara);
                }
                foreach (var thing in chara.things)
                {
                    if (onlyShop)
                    {
                        if (thing.trait is TraitChestMerchant)
                        {
                            foreach (var thing2 in thing.things)
                            {
                                SearchAndAdd(thing2);
                            }
                        }
                    }
                    else
                    {
                        SearchThing(thing);
                    }
                }
            }
        }

        private void SearchMap()
        {
            foreach (Thing thing in EMono._map.things)
            {
                SearchThing(thing);
            }
        }

        private void SearchThing(Thing thing)
        {
            SearchAndAdd(thing);
            if (thing.trait is TraitContainer)
            {
                foreach (var thing2 in thing.things)
                {
                    SearchThing(thing2);
                }
            }
        }
    }
}
