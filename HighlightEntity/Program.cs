using NXOpen;
using NXOpenUI;
using NXOpen.UF;
using NXOpen.Assemblies;
using System;


namespace HighlightEntity
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Session oSession = NXOpen.Session.GetSession();
            Part oActivePart = oSession.Parts.Work;
            PartCleanup();
            if (oActivePart != null)
            {
                if (!(oActivePart.ComponentAssembly.RootComponent is null))
                {
                    foreach (Component oChildComp in oActivePart.ComponentAssembly.RootComponent.GetChildren())
                    {
                        Part oActiveChildPart = GetPartFromComp(oChildComp);
                        HighlightFace(oActiveChildPart);
                    }
                }
                else
                {
                    HighlightFace(oActivePart);
                }
            }

            //Local Function
            void HighlightFace(Part oPart)
            {
                if (oPart != null)
                {
                    oPart.LoadThisPartFully();
                    foreach (Body oBody in oPart.Bodies)
                    {
                        foreach (Face oFace in oBody.GetFaces())
                        {
                            foreach (Edge oEdge in oFace.GetEdges())
                            {
                                if ((oEdge.GetLength() >= 2.5) && (oEdge.GetLength() <= 3.5))
                                {
                                    oFace.Highlight();
                                    UFSession.GetUFSession().Disp.Refresh();
                                    break;
                                }
                            }
                        }

                    }
                }
            }

        }

        public static int GetUnloadOption(string dummy)
        {
            int output;
            output = System.Convert.ToInt32(NXOpen.Session.LibraryUnloadOption.Immediately);
            return output;
        }

        public static void PartCleanup()
        {
            PartCleanup oPartClean = null;
            oPartClean = Session.GetSession().NewPartCleanup();
            oPartClean.TurnOffHighlighting = true;
            oPartClean.DoCleanup();
            oPartClean.Dispose();
        }

        public static Part GetPartFromComp(Component oComp)
        {

            Tag Part_Tag;
            UFSession.GetUFSession().Part.AskTagOfDispName(oComp.DisplayName, out Part_Tag);

            foreach (Part oPart in Session.GetSession().Parts)
            {
                if (oPart.Tag == Part_Tag)
                {
                    return oPart;
                    break;
                }

            }
            return null;
        }
    }


}
