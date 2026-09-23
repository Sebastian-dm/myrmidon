namespace Myrmidon.Core.Actions;

public class ActionResult {
    public bool Succeeded;
    public IAction? Alternative;

    public ActionResult() { 

    }

    public ActionResult(bool succeeded) {
        Succeeded = succeeded;
    }

    public ActionResult(bool succeeded, IAction alternative) {
        Succeeded = succeeded;
        Alternative = alternative;
    }

}