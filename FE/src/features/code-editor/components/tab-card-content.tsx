import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card.tsx";

type TabCardContentProps = {
  title: string | undefined;
  description: string | undefined;
};

export default function TabCardContent({
  title,
  description,
}: TabCardContentProps) {
  return (
    <Card className="h-[99%] w-[95%]">
      <CardHeader>
        <CardTitle>{title}</CardTitle>
      </CardHeader>
      <CardContent>
        <p>{description}</p>
      </CardContent>
    </Card>
  );
}
