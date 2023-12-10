import { ReactNode, useEffect } from "react";
import { useDispatch } from "react-redux";
import { setAppState } from "../../redux/actions/appStateActions";

interface PageWrapperProps {
  state?: string;
  children: ReactNode;
}

const PageWrapper = ({ state, children }: PageWrapperProps) => {

  const dispatch = useDispatch();
  useEffect(() => {
    if (state) {
      dispatch(setAppState(state));
    }
  }, [dispatch, state]);

  return (
    <>{children}</>
  );
};

export default PageWrapper;