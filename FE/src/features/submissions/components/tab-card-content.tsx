import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card.tsx";

type TabCardContentProps = {
  title: string | undefined;
  description: string | undefined;
  stdInList: string[] | undefined;
  expectedOutputList: string[] | undefined;
};

export default function TabCardContent({
  title,
  description,
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
        <div className="h-1/2 justify-end flex flex-col">
          {Array.from({ length: 1 }).map((_item, index) => (
            <div key={index} className="flex flex-col mt-8">
              <p className="font-bold mb-1">Example {index+1}</p>
              <p>Input: {stdInList?.[index]}</p>
              <p>
                Output: {expectedOutputList?.[index]?.split(",")[index]}
              </p>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}
