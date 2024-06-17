import * as React from "react"
import * as AccordionPrimitive from "@radix-ui/react-accordion"
import { ChevronDown } from "lucide-react"

import { cn } from "@/lib/utils"

const Accordion = AccordionPrimitive.Root

const AccordionItem = React.forwardRef(({ className, ...props }, ref) => (
  <AccordionPrimitive.Item
    ref={ref}
    className={cn("tailwind.config.jsborder-b", className)}
    {...props} />
))
AccordionItem.displayName = "AccordionItem"

const AccordionTrigger = React.forwardRef(({ className, children, ...props }, ref) => (
  <AccordionPrimitive.Header className="tailwind.config.jsflex">
    <AccordionPrimitive.Trigger
      ref={ref}
      className={cn(
        "tailwind.config.jsflex tailwind.config.jsflex-1 tailwind.config.jsitems-center tailwind.config.jsjustify-between tailwind.config.jspy-4 tailwind.config.jsfont-medium tailwind.config.jstransition-all hover:tailwind.config.jsunderline [&[data-state=open]>svg]:tailwind.config.jsrotate-180",
        className
      )}
      {...props}>
      {children}
      <ChevronDown
        className="tailwind.config.jsh-4 tailwind.config.jsw-4 tailwind.config.jsshrink-0 tailwind.config.jstransition-transform tailwind.config.jsduration-200" />
    </AccordionPrimitive.Trigger>
  </AccordionPrimitive.Header>
))
AccordionTrigger.displayName = AccordionPrimitive.Trigger.displayName

const AccordionContent = React.forwardRef(({ className, children, ...props }, ref) => (
  <AccordionPrimitive.Content
    ref={ref}
    className="tailwind.config.jsoverflow-hidden tailwind.config.jstext-sm tailwind.config.jstransition-all data-[state=closed]:tailwind.config.jsanimate-accordion-up data-[state=open]:tailwind.config.jsanimate-accordion-down"
    {...props}>
    <div
      className={cn("tailwind.config.jspb-4 tailwind.config.jspt-0", className)}>{children}</div>
  </AccordionPrimitive.Content>
))

AccordionContent.displayName = AccordionPrimitive.Content.displayName

export { Accordion, AccordionItem, AccordionTrigger, AccordionContent }
