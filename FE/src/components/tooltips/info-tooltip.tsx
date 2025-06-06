import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "@/components/ui/tooltip";
import { type LucideIcon } from "lucide-react";
import { cn } from "@/lib/utils.ts";

type InfoTooltipProps = {
  icon: LucideIcon;
  tooltipContent: string
  className?: string;
};

export default function InfoTooltip({ tooltipContent, className, ...props }: InfoTooltipProps) {
  return (
    <TooltipProvider>
      <Tooltip>
        <TooltipTrigger disabled className={cn("w-fit", className)}>
          {<props.icon width="22px" height="22px" />}
        </TooltipTrigger>
        <TooltipContent>
          <p>{tooltipContent}</p>
        </TooltipContent>
      </Tooltip>
    </TooltipProvider>
  );
}
