import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table.tsx";
import { LANGUAGE_ID_TO_NAME } from "@/utils/constants/language-id-to-name.ts";
import { type TeacherLastUserSubmissionsDto } from "@/features/submissions/types/submissions-types.ts";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

type ProblemStatisticsTableProps = {
  data: TeacherLastUserSubmissionsDto[] | undefined;
  selectValue: string;
  setSelectValue: (value: string) => void;
  setQueryParamValue: (numOfSubmissions: string) => void;
};

export default function LastUserSubmissionsTable({
  data,
  selectValue,
  setSelectValue,
  setQueryParamValue,
}: ProblemStatisticsTableProps) {

  function handleSelectValueChange(value: string) {
    setSelectValue(value)
    setQueryParamValue(value);
  }

  return (
    <>
      <Select
        value={selectValue!}
        onValueChange={handleSelectValueChange}
      >
        <SelectTrigger className="w-[180px] ms-11 mb-3">
          <SelectValue placeholder="№ of submissions" />
        </SelectTrigger>
        <SelectContent>
          <SelectGroup>
            <SelectLabel>Number of submissions to show</SelectLabel>
            <SelectItem value="10">10</SelectItem>
            <SelectItem value="20">20</SelectItem>
            <SelectItem value="30">30</SelectItem>
            <SelectItem value="40">40</SelectItem>
            <SelectItem value="50">50</SelectItem>
          </SelectGroup>
        </SelectContent>
      </Select>
      <Table className="w-[95%] mx-auto">
        <TableHeader>
          <TableRow>
            <TableHead className="w-[100px]">#</TableHead>
            <TableHead>Student</TableHead>
            <TableHead>Problem</TableHead>
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
              <TableCell>{item.user}</TableCell>
              <TableCell>{item.problemId}</TableCell>
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
    </>
  );
}
