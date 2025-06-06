import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card.tsx";

type TabCardContentProps = {
  title: string | undefined;
  description: string | undefined;
  requiredPercentageToPass: number | undefined;
  stdInList: string[] | undefined;
  expectedOutputList: string[] | undefined;
};

export default function TabCardContent({
  title,
  description,
  requiredPercentageToPass,
  stdInList,
  expectedOutputList,
}: TabCardContentProps) {
  return (
    <Card className="h-[99%] w-[95%]">
      <CardHeader>
        <CardTitle>{title}</CardTitle>
      </CardHeader>
      <CardContent className="flex flex-col h-full">
        <p>{description}</p>
        <div className="h-1/2 justify-end flex flex-col mb-5">
          {Array.from({ length: 1 }).map((_item, index) => (
            <div key={index} className="flex flex-col mt-8">
              <p className="font-bold mb-1">Example {index + 1}</p>
              <p>Input: {stdInList?.[index]}</p>
              <p>Output: {expectedOutputList?.[index]?.split(",").join(",")}</p>
            </div>
          ))}
        </div>
        <div>
          <p className="font-bold">
            Required % to pass: <u>{requiredPercentageToPass}%</u>
          </p>
        </div>
      </CardContent>
    </Card>
  );
}
