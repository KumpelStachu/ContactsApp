import { useGetQuery } from '@/api/hooks'
import ContactRow from '@/components/ContactRow'
import TableSkeleton from '@/components/TableSkeleton'
import { useAutoAnimate } from '@formkit/auto-animate/react'
import { createLazyFileRoute } from '@tanstack/react-router'
import { ChevronLeftIcon, ChevronRightIcon } from 'lucide-react'
import { useEffect, useState } from 'react'

export const Route = createLazyFileRoute('/')({
	component: function Index() {
		const [tableRef] = useAutoAnimate()
		const [pageIndex, setPageIndex] = useState(1)
		const { data, error, isLoading, isFetching } = useGetQuery('/Contact', {
			params: { query: { pageIndex } },
		})

		useEffect(() => {
			if (isFetching) return
			setPageIndex(page => Math.min(Math.max(page, 1), data?.totalPages ?? 1))
		}, [data, isFetching])

		if (error) {
			return <div>Error: {error.message}</div>
		}

		return (
			<div className="card bg-base-100 shadow-md">
				<div className="card-body">
					<div className="card-title space-x-2">
						Lista kontaktów
						{isFetching && <span className="loading loading-dots"></span>}
					</div>
					<table className="table">
						<thead>
							<tr className="[]:w-full">
								<th>Imię</th>
								<th>Nazwisko</th>
								<th>Numer telefonu</th>
								<th className="w-0"></th>
							</tr>
						</thead>
						<tbody ref={tableRef}>
							{isLoading || !data ? (
								<TableSkeleton rows={3} />
							) : (
								data.items.map(contact => <ContactRow key={contact.id} contact={contact} />)
							)}
						</tbody>
					</table>
					<div className="card-actions">
						<div className="join mx-auto">
							<button
								className="join-item btn btn-square"
								onClick={() => setPageIndex(page => page - 1)}
								disabled={!data?.hasPreviousPage}
							>
								<ChevronLeftIcon className="w-4 h-4" />
							</button>
							<button className="join-item btn pointer-events-none">
								Strona{' '}
								{isLoading || !data ? (
									<div className="skeleton w-8 h-4" />
								) : (
									`${data.pageIndex}/${data.totalPages}`
								)}
							</button>
							<button
								className="join-item btn btn-square"
								onClick={() => setPageIndex(page => page + 1)}
								disabled={!data?.hasNextPage}
							>
								<ChevronRightIcon className="w-4 h-4" />
							</button>
						</div>
					</div>
				</div>
			</div>
		)
	},
})
