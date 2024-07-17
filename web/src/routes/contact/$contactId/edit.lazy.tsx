import { createLazyFileRoute } from '@tanstack/react-router'

export const Route = createLazyFileRoute('/contact/$contactId/edit')({
  component: () => <div>Hello /contact/$contactId/edit!</div>
})