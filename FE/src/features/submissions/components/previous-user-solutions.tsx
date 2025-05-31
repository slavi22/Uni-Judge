import { Card, CardContent, CardHeader } from "@/components/ui/card.tsx";
import { type ProblemUserSubmissionsDto } from "@/features/submissions/types/submissions-types.ts";
import { LANGUAGE_ID_TO_NAME } from "@/utils/constants/language-id-to-name.ts";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

type PreviousUserSolutionsProps = {
  data: ProblemUserSubmissionsDto[] | undefined;
};

export default function PreviousUserSolutions({
  data,
}: PreviousUserSolutionsProps) {
  return (
    <Card className="h-[99%] w-[95%]">
      <CardHeader>
        Here are your previous submissions for this problem
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[100px]">#</TableHead>
              <TableHead>Is Passing?</TableHead>
              <TableHead>Language</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {[...data!]?.reverse().map((item, index) => (
              <TableRow
                key={item.submissionId}
                className="odd:bg-gray-100 even:bg-white dark:odd:bg-zinc-800 dark:even:bg-zinc-700"
              >
                <TableCell className="font-medium">
                  {data!.length - index}
                </TableCell>
                <TableCell>
                  {item.isPassing
                    ? "✅ Correct"
                    : item.isError
                      ? `❌ ${item.errorResult}`
                      : "❌ Wrong answer"}
                </TableCell>
                <TableCell>
                  {LANGUAGE_ID_TO_NAME[item.languageId].language}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
}
