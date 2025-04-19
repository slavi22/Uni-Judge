export default function splitLocationPathname(pathname: string) {
  const pathnameSplit = pathname.split("/").filter((path) => path !== "");
  const pathnameSplitWithoutLastElement = pathnameSplit.slice(
    0,
    pathnameSplit.length - 1,
  );
  const pathnameSplitLastElement = pathnameSplit[pathnameSplit.length - 1];
  return { pathnameSplit, pathnameSplitWithoutLastElement, pathnameSplitLastElement };
}
