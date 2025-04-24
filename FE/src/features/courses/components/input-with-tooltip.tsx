import { Input } from "@/components/ui/input.tsx";
import InfoTooltip from "@/components/tooltips/info-tooltip.tsx";
import { type LucideIcon } from "lucide-react";
import { type ComponentProps } from "react";

type InputWithTooltipProps = {
  icon: LucideIcon;
  tooltipContent: string;
} & ComponentProps<"input">;

export default function InputWithTooltip({
  icon,
  tooltipContent,
  ...props
}: InputWithTooltipProps) {
  return (
    <div className="flex gap-3 items-center relative">
      <Input placeholder="Course idetifier" {...props} />
      <InfoTooltip icon={icon} tooltipContent={tooltipContent} className="absolute right-1" />
    </div>
  );
}
